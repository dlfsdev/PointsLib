#include "stdafx.h"
#include "ForgyKMeansInitialMeansStrategy.h"

#include "Point.h"
#include "KMeansHelpers.h"

#include <algorithm>
#include <iterator>
#include <random>
#include <set>

using namespace PointsLib::KMeansHelpers;

namespace PointsLib
{

namespace
{
    class RandomPartitionKMeansInitialMeansStrategy : public IKMeansInitialMeansStrategy
    {
    public:
        virtual std::vector<Point> SelectInitialMeans(int k, const std::vector<Point>& points) const override
        {
            if(k <= 0)
                throw std::exception("k must be > 0");

            const size_t n = points.size();

            std::default_random_engine randEngine(std::random_device().operator()());
            std::uniform_int_distribution<int> randomCluster(0, k - 1);

            std::vector<int> assignments(n);
            for(size_t i = 0; i < n; ++i)
                assignments[i] = randomCluster(randEngine);

            return ComputeCentroids(k, points, assignments);
        }
    };
}



IKMeansInitialMeansStrategy* CreateRandomPartitionKMeansInitialMeansStrategy()
{
    return new RandomPartitionKMeansInitialMeansStrategy();
}

}