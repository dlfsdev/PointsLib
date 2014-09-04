#include "stdafx.h"
#include "ForgyKMeansInitialMeansStrategy.h"

#include "Point.h"

#include <algorithm>
#include <iterator>
#include <random>
#include <set>

namespace PointsLib
{

namespace
{
    class ForgyKMeansInitialMeansStrategy : public IKMeansInitialMeansStrategy
    {
    public:
        virtual std::vector<Point> SelectInitialMeans(int k, const std::vector<Point>& points) const override
        {
            if(k <= 0)
                throw std::exception("k must be > 0");

            std::default_random_engine randEngine(std::random_device().operator()());
            std::uniform_int_distribution<size_t> randomPointIndex(0, points.size() - 1);

            std::set<size_t> indexes;
            while(indexes.size() < static_cast<size_t>(k))
                indexes.insert(randomPointIndex(randEngine));

            std::vector<Point> means;
            std::transform(indexes.begin(), indexes.end(), std::back_inserter(means), [&](size_t i) { return points[i]; });
            return means;
        }
    };
}

IKMeansInitialMeansStrategy* CreateForgyKMeansInitialMeansStrategy()
{
    return new ForgyKMeansInitialMeansStrategy();
}

}