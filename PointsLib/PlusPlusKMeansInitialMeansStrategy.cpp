#include "stdafx.h"
#include "ForgyKMeansInitialMeansStrategy.h"

#include "Point.h"
#include "PointExtensions.h"

#include <assert.h>
#include <numeric>
#include <random>

namespace PointsLib
{

namespace
{
    class PlusPlusKMeansInitialMeansStrategy : public IKMeansInitialMeansStrategy
    {
    private:
        mutable std::default_random_engine m_randEngine;

    public:
        PlusPlusKMeansInitialMeansStrategy()
            : m_randEngine(std::random_device().operator()())
        {
        }

        virtual std::vector<Point> SelectInitialMeans(int k, const std::vector<Point>& points) const override
        {
            if(k <= 0)
                throw std::exception("k must be > 0");

            std::vector<Point> means;
            means.reserve(k);
            
            std::vector<double> weights(points.size());

            while(means.size() < static_cast<size_t>(k))
            {
                for(size_t i = 0; i < points.size(); ++i)
                    weights[i] = means.empty() ? 1.0 : SquaredDistanceToNearestMean(points[i], means);
                size_t selected = WeightedRandomIndex(weights);
                means.push_back(points[selected]);
            }

            return means;
        }

        double SquaredDistanceToNearestMean(const Point& p, const std::vector<Point>& means) const
        {
            size_t index = FindNearestPoint(p, means);
            return SquaredDistanceBetween(p, means[index]);
        }

        size_t WeightedRandomIndex(const std::vector<double>& weights) const
        {
            double sumWeights = std::accumulate(weights.begin(), weights.end(), 0.0);

            std::uniform_real_distribution<double> randomValue(0.0, sumWeights);
            double randomNumber = randomValue(m_randEngine);

            double weightConsumed = 0.0;
            for(size_t i = 0; i < weights.size(); ++i)
            {
                weightConsumed += weights[i];
                if(randomNumber < weightConsumed)
                    return i;
            }

            assert(false);
            return weights.size() - 1;
        }
    };
}

IKMeansInitialMeansStrategy* CreatePlusPlusKMeansInitialMeansStrategy()
{
    return new PlusPlusKMeansInitialMeansStrategy();
}

}