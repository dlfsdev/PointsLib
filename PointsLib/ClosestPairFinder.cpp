#include "stdafx.h"
#include "ClosestPairFinder.h"

#include "Nullable.h"
#include "Point.h"
#include "PointExtensions.h"
#include "PointsAlgorithmObservers.h"
#include "RunObserverableAlgorithm.h"

#include <algorithm>
#include <assert.h>
#include <exception>
#include <limits>

using namespace PointsLib::Private;
using namespace Util;

namespace PointsLib
{

namespace
{
    class ClosestPairFinderImpl : public ClosestPairFinder
    {
    private:
        static const size_t MAX_POINTS_FOR_BRUTE_FORCE = 3;
        
        typedef std::vector<Point>::iterator PointIt;
        typedef std::vector<Point>::const_iterator PointConstIt;

        struct ClosePair
        {
            ClosePair(const Point& p1, const Point& p2, double distance_)
                : points(std::vector<Point> { p1, p2 })
                , distance(distance_)
            {
            }

            std::vector<Point> points;
            double distance;
        };

    private:
        mutable PointsAlgorithmObservers m_observers;

    public:
        ClosestPairFinderImpl()
            : ClosestPairFinder()
        {
        }

        ~ClosestPairFinderImpl()
        {
        }

        virtual std::vector<Point> Solve(const std::vector<Point>& input) const override
        {
            if(input.size() < 2)
                throw std::exception("Must provide at least two points");

            std::vector<Point> points(input);
            std::sort(points.begin(), points.end(), CompareByX);
            int stepsPerformed = 0;
            const int totalSteps = ComputeTotalReports(points.size());

            return RunObservableAlgorithm(
                [&]()
                {
                    return SolveImpl(points.begin(), points.end(), stepsPerformed, totalSteps).points;
                }, m_observers );
        }

        virtual bool ReportsDefiniteProgress() const override
        {
            return true;
        }

        virtual void AddObserver(const std::weak_ptr<IPointsAlgorithmObserver>& observer) const override
        {
            m_observers.AddObserver(observer);
        }
        
        virtual void RemoveObserver(const IPointsAlgorithmObserver& observer) const override
        {
            m_observers.RemoveObserver(observer);
        }

    private:

        ClosePair SolveImpl(PointConstIt itBegin, PointConstIt itEnd, int& stepsCompleted, int totalSteps) const
        {
            ClosePair answer = SolveImplImpl(itBegin, itEnd, stepsCompleted, totalSteps);
            ReportProgress(m_observers, ++stepsCompleted, totalSteps, answer.points);
            return answer;                
        }

        // (sorry)
        ClosePair SolveImplImpl(PointConstIt itBegin, PointConstIt itEnd, int& stepsCompleted, int totalSteps) const
        {
            size_t numPoints = std::distance(itBegin, itEnd);

            if(numPoints <= MAX_POINTS_FOR_BRUTE_FORCE)
                return FindClosestBruteForce(itBegin, itEnd);

            auto itMid = itBegin + numPoints / 2;

            ClosePair closestOnLeft = SolveImpl(itBegin, itMid, stepsCompleted, totalSteps);
            ClosePair closestOnRight = SolveImpl(itMid, itEnd, stepsCompleted, totalSteps);

            const ClosePair* closestOnEitherSide = closestOnLeft.distance <= closestOnRight.distance ?
                &closestOnLeft : &closestOnRight;

            std::vector<Point> pointsNearDivider = GetPointsNearDivider(itBegin, itEnd, itMid, closestOnEitherSide->distance);

            Nullable<ClosePair> closestAcrossDivider =
                FindClosestBounded(pointsNearDivider.begin(), pointsNearDivider.end(), closestOnEitherSide->distance);

            return closestAcrossDivider.HasValue() ? closestAcrossDivider.Value() : *closestOnEitherSide;
        }

        static std::vector<Point> GetPointsNearDivider(PointConstIt itBegin, PointConstIt itEnd, PointConstIt itDivider,
            double nearerThan)
        {
            // since the points are sorted by x-coordinate, we can just start at the divider and scan
            // in both directions until we get more than nearerThan away

            std::vector<Point> pointsNearDivider;

            for(auto it = itDivider; ; --it)
            {
                double xDistance = itDivider->x - it->x;
                assert(xDistance >= 0.0);

                if(xDistance >= nearerThan)
                    break;

                pointsNearDivider.push_back(*it);

                if(it == itBegin)
                    break;
            }

            for(auto it = itDivider + 1; it != itEnd; ++it)
            {
                double xDistance = it->x - itDivider->x;
                assert(xDistance >= 0.0);

                if(xDistance >= nearerThan)
                    break;

                pointsNearDivider.push_back(*it);
            }

            return pointsNearDivider;
        }

        // Input range will be sorted by y-coordinate
        static Nullable<ClosePair> FindClosestBounded(PointIt itBegin, PointIt itEnd, double closerThan)
        {
            Nullable<ClosePair> answer;
            double disanceToBeat = closerThan;

            std::sort(itBegin, itEnd, CompareByY);

            for(auto i = itBegin; i != itEnd; ++i)
            {
                for(auto j = i + 1; j != itEnd; ++j)
                {
                    // since the points are sorted by y-coordinate, as soon as the y-distance
                    // exceeds closerThan, there's no reason to continue
                    if(j->y - i->y >= disanceToBeat)
                        break;

                    double distance = CalcDistance(*i, *j);
                    if(distance < disanceToBeat)
                    {
                        answer = ClosePair(*i, *j, distance);
                        disanceToBeat = distance;
                    }
                }
            }
            
            return answer;
        }

        static ClosePair FindClosestBruteForce(PointConstIt itBegin, PointConstIt itEnd)
        {
            ClosePair answer(Point(), Point(), std::numeric_limits<double>::max());

            for(auto i = itBegin; i != itEnd; ++i)
            {
                for(auto j = i + 1; j != itEnd; ++j)
                {
                    double distance = CalcDistance(*i, *j);
                    if(distance < answer.distance)
                        answer = ClosePair(*i, *j, distance);
                }
            }
            
            return answer;
        }

        static double CalcDistance(const Point& p1, const Point& p2)
        {
            // since we're only using the distances in comparisons,
            // we can keep them squared and save some effort
            return SquaredDistanceBetween(p1, p2);
        }

        static int ComputeTotalReports(size_t numPoints)
        {
            if(numPoints <= MAX_POINTS_FOR_BRUTE_FORCE)
                return 1;

            const size_t halfPoints = numPoints / 2;

            if(numPoints % 2 == 0)
                return 1 + 2 * ComputeTotalReports(halfPoints);
            else
                return 1 + ComputeTotalReports(halfPoints) + ComputeTotalReports(halfPoints + 1);
        }
    };
}



ClosestPairFinder::ClosestPairFinder()
{
}

ClosestPairFinder::~ClosestPairFinder()
{
}

ClosestPairFinder* ClosestPairFinder::Create()
{
    return new ClosestPairFinderImpl();
}

}