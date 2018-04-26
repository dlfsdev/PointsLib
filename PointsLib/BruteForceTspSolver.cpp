#include "stdafx.h"
#include "BruteForceTspSolver.h"

#include "Point.h"
#include "PointExtensions.h"
#include "PointsAlgorithmObservers.h"
#include "RunObserverableAlgorithm.h"

#include <algorithm>
#include <assert.h>
#include <functional>
#include <numeric>
#include <stdint.h>
#include <thread>

using namespace PointsLib::Private;

namespace PointsLib
{

namespace
{

inline uint64_t Factorial(uint64_t n)
{
    return n <= 1 ? 1 : n * Factorial(n - 1);
}

// thanks stackoverflow
std::vector<size_t> GetPermutationNumber(int n, uint64_t permutationNumber)
{
    assert(n >= 0);

    std::vector<uint64_t> factorialNumbers(n);
    factorialNumbers[0] = 1;
    for(int i = 1; i < n; ++i)
        factorialNumbers[i] = i * factorialNumbers[i - 1];

    uint64_t j = permutationNumber;
    std::vector<size_t> permutation(n);
    for(int i = 0; i < n; ++i)
    {
        permutation[i] = static_cast<size_t>(j / factorialNumbers[n - 1 - i]);
        j = j % factorialNumbers[n - 1 - i];
    }

    for (int i = n - 1; i > 0; --i)
        for (int j2 = i - 1; j2 >= 0; --j2)
            if (permutation[j2] <= permutation[i])
                permutation[i]++;

    return permutation;
}

template<typename T>
std::vector<T> GetPermutationNumber(const std::vector<T>& values, uint64_t permutationNumber)
{
    std::vector<size_t> permutationIndexes = GetPermutationNumber(static_cast<int>(values.size()), permutationNumber);
    
    std::vector<T> firstPermutation = values;
    std::sort(firstPermutation.begin(), firstPermutation.end());

    std::vector<T> answer;
    answer.reserve(values.size());
    
    for(size_t nextIndexInPermutation : permutationIndexes)
        answer.push_back(firstPermutation[nextIndexInPermutation]);

    return answer;
}

inline double ComputePathLength(const std::vector<Point>& path)
{
    if(path.size() < 2)
        return 0.0;
    double length = 0.0;
    for(size_t i = 1; i < path.size(); ++i)
        length += DistanceBetween(path[i - 1], path[i]);
    length += DistanceBetween(path.back(), path.front());
    return length;
}



struct TspProgress
{
    TspProgress(double fractionExamined, const std::vector<Point>& path, double pathLength)
        : FractionExamined(fractionExamined)
        , Path(path)
        , PathLength(pathLength)
    {
    }

    double FractionExamined;
    std::vector<Point> Path;
    double PathLength;
};



class SubSolver
{
    std::function<void(const TspProgress&)> m_observer;

public:
    SubSolver(const std::function<void(const TspProgress&)>& observer)
        : m_observer(observer)
    {
    }

    void Solve(const std::vector<Point>& points, uint64_t numPermutationsToExamine) const
    {
        std::vector<Point> bestPathEncountered;
        double bestLengthEncountered = std::numeric_limits<double>::max();

        std::vector<Point> candidate = points;

        for(uint64_t thisIteration = 0; thisIteration < numPermutationsToExamine; ++thisIteration)
        {
            const double thisLength = ComputePathLength(candidate);
            if(thisLength < bestLengthEncountered)
            {
                bestPathEncountered = candidate;
                bestLengthEncountered = thisLength;
            }

            if(thisIteration == numPermutationsToExamine - 1 || thisIteration % 10000 == 0)
                ReportProgress(thisIteration + 1, numPermutationsToExamine, bestPathEncountered, bestLengthEncountered);

            std::next_permutation(candidate.begin(), candidate.end());
        }
    }

private:
    void ReportProgress(uint64_t iterationNumber, uint64_t totalIterations, const std::vector<Point>& provisionalAnswer, double provisionalAnswerLength) const
    {
        m_observer(TspProgress((double)iterationNumber / totalIterations, provisionalAnswer, provisionalAnswerLength));
    }
};



class ProgressAggregator
{
private:
    const std::function<void(const TspProgress&)> m_listener;
    
    mutable std::mutex m_locker;

    std::vector<double> m_perWorkerProgress;

    std::vector<Point> m_bestPath;
    double m_bestPathLength;

public:
    ProgressAggregator(int numberToAggregate, const std::function<void(const TspProgress&)>& listener)
        : m_listener(listener)
        , m_perWorkerProgress(numberToAggregate)
        , m_bestPath()
        , m_bestPathLength(std::numeric_limits<double>::max())
    {
    }

    std::vector<Point> GetBestPath() const
    {
        std::lock_guard<std::mutex> lock(m_locker);
        return m_bestPath;
    }

    double GetBestPathLength() const
    {
        std::lock_guard<std::mutex> lock(m_locker);
        return m_bestPathLength;
    }
   
    void OnProgress(int workerNumber, const TspProgress& progress)
    {
        TspProgress aggregateProgress = OnProgressImpl(workerNumber, progress);
        m_listener(aggregateProgress);
    }

private:
    TspProgress OnProgressImpl(int workerNumber, const TspProgress& workerProgress)
    {
        std::lock_guard<std::mutex> lock(m_locker);

        m_perWorkerProgress[workerNumber] = workerProgress.FractionExamined;

        if(workerProgress.PathLength < m_bestPathLength)
        {
            m_bestPath = workerProgress.Path;
            m_bestPathLength = workerProgress.PathLength;
        }

        const double totalComplete = std::accumulate(m_perWorkerProgress.begin(), m_perWorkerProgress.end(), 0.0) / m_perWorkerProgress.size();
        return TspProgress(totalComplete, m_bestPath, m_bestPathLength);
    }
};
 


class BruteForceTspSolverImpl : public BruteForceTspSolver
{
private:
    mutable PointsAlgorithmObservers m_observers;
    const int m_numThreads;

public:
    BruteForceTspSolverImpl(int numThreads)
        : BruteForceTspSolver()
        , m_numThreads(numThreads)
    {
    }

    ~BruteForceTspSolverImpl()
    {
    }

    virtual std::vector<Point> Solve(const std::vector<Point>& points) const override
    {
        return RunObservableAlgorithm( [&]() { return SolveImpl(points, m_numThreads); }, m_observers );
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
    std::vector<Point> SolveImpl(const std::vector<Point>& points, int numThreads) const
    {
        if(points.size() <= 3)
            return points;

        const uint64_t numPermutations = Factorial(points.size() - 1); // leave the first point fixed to avoid equivalent rotations
        const uint64_t permutationsPerThread = numPermutations / numThreads;
        const uint64_t permutationsForLastThread = permutationsPerThread + numPermutations % numThreads;
        
        ProgressAggregator progressAggregator(numThreads,
            [&](const TspProgress& progress) { ReportProgress(m_observers, progress.FractionExamined, progress.Path); });

        std::vector<std::thread> threads;
        for(int i = 0; i < numThreads; ++i)
        {
            SubSolver solver([=, &progressAggregator](const TspProgress& progress) { progressAggregator.OnProgress(i, progress); });
            std::vector<Point> startPermutation = GetPermutationNumber(points, i * permutationsPerThread);
            uint64_t numPermutationsThisThread = i == numThreads - 1 ? permutationsForLastThread : permutationsPerThread;
            threads.emplace_back([=]() { try { solver.Solve(startPermutation, numPermutationsThisThread); } catch(...) {} }); // TODO: Get that exception
        }

        for(auto& thread : threads)
            thread.join();

        return progressAggregator.GetBestPath();
    }
};

}



BruteForceTspSolver* BruteForceTspSolver::Create(int numThreads)
{
    return new BruteForceTspSolverImpl(numThreads);
}

BruteForceTspSolver::BruteForceTspSolver()
{
}

BruteForceTspSolver::~BruteForceTspSolver()
{
}

}