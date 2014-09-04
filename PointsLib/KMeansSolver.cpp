#include "stdafx.h"
#include "KMeansSolver.h"

#include "IKMeansInitialMeansStrategy.h"
#include "KMeansHelpers.h"
#include "Point.h"
#include "PointExtensions.h"
#include "PointsAlgorithmObservers.h"
#include "RunObserverableAlgorithm.h"

#include <assert.h>
#include <limits>

using namespace PointsLib::Private;
using namespace PointsLib::KMeansHelpers;

namespace PointsLib
{

namespace
{
    class KMeansSolverImpl : public KMeansSolver
    {
    private:
        const int m_k;
        const int m_repetitions;
        const std::shared_ptr<const IKMeansInitialMeansStrategy> m_initialMeansStrategy;

        mutable PointsAlgorithmObservers m_observers;

    private:
        struct IterationResult
        {
            std::vector<Point> means; // locations of the cluster centroids
            std::vector<int> assignments; // indexes of the clusters each point was assigned to
        };

    public:
        KMeansSolverImpl(int k, int repetitions,
            const std::shared_ptr<const IKMeansInitialMeansStrategy>& initialMeansStrategy)
            : KMeansSolver()
            , m_k(k)
            , m_repetitions(repetitions)
            , m_initialMeansStrategy(initialMeansStrategy)
        {
            if(m_k <= 0)
                throw new std::exception("k must be > 0");

            if(m_repetitions <= 0)
                throw new std::exception("repetitions must be > 0");
        }

        virtual std::vector<Point> Solve(const std::vector<Point>& points) const override
        {
            if(points.size() < static_cast<size_t>(m_k))
                throw std::exception("too few points");

            return RunObservableAlgorithm(
                [&]() { return SolveImpl(points, m_k, m_repetitions, *m_initialMeansStrategy); },
                m_observers);
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
        std::vector<Point> SolveImpl(
            const std::vector<Point>& points,
            int k,
            int repetitions,
            const IKMeansInitialMeansStrategy& initialMeansStrategy) const
        {
            IterationResult bestResult;
            double bestScore = std::numeric_limits<double>::max();

            for(int r = 0; r < repetitions; ++r)
            {
                IterationResult thisResult = SolveSingleIteration(k, points,
                    initialMeansStrategy.SelectInitialMeans(k, points));

                const double score = ComputeScore(points, thisResult);
                if(score < bestScore)
                {
                    bestScore = score;
                    bestResult = thisResult;
                }

                ReportProgress(m_observers, r + 1, repetitions);
            }

            return bestResult.means;
        }

        IterationResult SolveSingleIteration(
            int k,
            const std::vector<Point>& points,
            const std::vector<Point>& initialMeans) const
        {
            const size_t n = points.size();

            IterationResult result;
            result.means = initialMeans;
            result.assignments = std::vector<int>(n, std::numeric_limits<int>::max());

            for(;;)
            {
                ReportProgress(m_observers, result.means);

                bool didAnyAssignmentsChange = false;

                for(size_t i = 0; i < n; ++i)
                {
                    int assignedCluster = static_cast<int>(FindNearestPoint(points[i], result.means));
                    if(result.assignments[i] == assignedCluster)
                        continue;
                    result.assignments[i] = assignedCluster;
                    didAnyAssignmentsChange = true;
                }

                if(!didAnyAssignmentsChange)
                    break;

                result.means = ComputeCentroids(k, points, result.assignments);
            }

            return result;
        }

        double ComputeScore(
            const std::vector<Point>& points,
            const IterationResult& result) const
        {
            const size_t n = points.size();

            assert(result.assignments.size() == n);

            double score = 0.0;
            for(size_t i = 0; i < n; ++i)
            {
                const int assignment = result.assignments[i];
                assert(assignment >= 0 && static_cast<size_t>(assignment) < result.means.size());
                score += SquaredDistanceBetween(points[i], result.means[assignment]);
            }

            return score;
        }
    };
}



KMeansSolver::KMeansSolver()
{
}

KMeansSolver::~KMeansSolver()
{
}

KMeansSolver* KMeansSolver::Create(int k, int repetitions,
    const std::shared_ptr<const IKMeansInitialMeansStrategy>& initialMeansStrategy)
{
    return new KMeansSolverImpl(k, repetitions, initialMeansStrategy);
}

}