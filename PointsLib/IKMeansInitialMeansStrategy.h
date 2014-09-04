#pragma once

#include <vector>

namespace PointsLib { struct Point; }

namespace PointsLib
{

struct IKMeansInitialMeansStrategy
{
public:
    virtual ~IKMeansInitialMeansStrategy() {}
    virtual std::vector<Point> SelectInitialMeans(int k, const std::vector<Point>& points) const = 0;
};

}