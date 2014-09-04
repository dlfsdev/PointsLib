#include "stdafx.h"
#include "QuickHullSolver.h"

#include "Point.h"
#include "PointExtensions.h"
#include "PointsAlgorithmObservers.h"
#include "RunObserverableAlgorithm.h"
#include "StdUtil.h"

#include <algorithm>
#include <assert.h>
#include <iterator>
#include <list>
#include <type_traits>
#include <vector>

using namespace PointsLib::Private;
using namespace Util;

namespace PointsLib
{

namespace
{
    class QuickHullSolverImpl : public QuickHullSolver
    {
    private:
        mutable PointsAlgorithmObservers m_observers;

    public:
        ~QuickHullSolverImpl()
        {
        }

        virtual std::vector<Point> Solve(const std::vector<Point>& points) const override
        {
            return RunObservableAlgorithm( [&]() { return SolveImpl(points); }, m_observers );
        }

        virtual bool ReportsDefiniteProgress() const override
        {
            return false;
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
        std::vector<Point> SolveImpl(const std::vector<Point>& points) const
        {
            if(points.empty())
                return std::vector<Point>();

            // Start by finding the two extremes in one dimension.
            // These points will definitely be part of the null.
            // With them as anchors, we can recursively search for the rest of the hull on each side of the line they define.
            Point minPoint, maxPoint;

            minPoint = *std::min_element(points.begin(), points.end(), CompareByX);
            maxPoint = *std::max_element(points.begin(), points.end(), CompareByX);
            
            if(minPoint.x == maxPoint.x)
            {
                // Degenerate case: points are in a vertical line.
                minPoint = *std::min_element(points.begin(), points.end(), CompareByY);
                maxPoint = *std::max_element(points.begin(), points.end(), CompareByY);

                if(minPoint != maxPoint)
                    return std::vector<Point> { minPoint, maxPoint };
                else // Extra-degenerate case: only 1 unique point.
                    return std::vector<Point> { minPoint };
            }

            std::list<Point> hull { minPoint, maxPoint };
            ReportProgress(m_observers, ToVector(hull));

            FindPointsInHull(minPoint, maxPoint, points, hull, ++hull.begin());
            FindPointsInHull(maxPoint, minPoint, points, hull, hull.end());

            return ToVector(hull);
        }

        // list's ability to keep iterators valid after insert will make it easier to keep track of where the next point should go
        void FindPointsInHull(const Point& partitionLine1, const Point& partitionLine2, const std::vector<Point>& points, std::list<Point>& hull,
            std::list<Point>::iterator itInsert) const
        {
            std::vector<Point> leftPoints = GetPointsLeftOf(partitionLine1, partitionLine2, points);
            if(leftPoints.empty())
                return;

            Point farthest = GetPointFarthestFrom(partitionLine1, partitionLine2, leftPoints);
            hull.insert(itInsert, farthest);
            ReportProgress(m_observers, ToVector(hull));

            FindPointsInHull(partitionLine1, farthest, leftPoints, hull, --MakeCopy(itInsert));
            FindPointsInHull(farthest, partitionLine2, leftPoints, hull, itInsert);
        }

        static Point GetPointFarthestFrom(const Point& l1, const Point& l2, const std::vector<Point>& points)
        {
            double maxDistance = -1.0;
            auto itMax = points.end();

            for(auto it = points.begin(); it != points.end(); ++it)
            {
                const double distance = DistanceToLine(*it, l1, l2);
                if(distance > maxDistance)
                {
                    maxDistance = distance;
                    itMax = it;
                }
            }

            return *itMax;
        }
        
        static std::vector<Point> GetPointsLeftOf(const Point& l1, const Point& l2, const std::vector<Point>& points)
        {
            std::vector<Point> result;
            std::copy_if(points.begin(), points.end(), std::back_inserter(result),
                [&](const Point& point) { return IsLeftOf(point, l1, l2); } );
            return result;
        }

        static bool IsLeftOf(const Point& point, const Point& l1, const Point& l2)
        {
            const double value = MagicDeterminate(point, l1, l2);
            return value > 0;
        }

        // > 0 => Left of line
        // < 0 => Right of line
        // = 0 => On the line
        static double MagicDeterminate(const Point& point, const Point& l1, const Point& l2)
        {
            const double value = (l2.x - l1.x) * (point.y - l1.y) - (l2.y - l1.y) * (point.x - l1.x);
            return value;
        }
    };
}



QuickHullSolver* QuickHullSolver::Create()
{
    return new QuickHullSolverImpl();
}

QuickHullSolver::QuickHullSolver()
{
}

QuickHullSolver::~QuickHullSolver()
{
}

}