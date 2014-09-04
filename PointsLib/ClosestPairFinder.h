#pragma once

#include "IPointsAlgorithm.h"

#include <vector>

namespace PointsLib
{

class ClosestPairFinder : public IPointsAlgorithm
{
public:
    static ClosestPairFinder* Create();
    virtual ~ClosestPairFinder();

    // points: Must contain at least two points
    // returns: vector for congruence with other algorithms, but will always contain exactly two points
    // throws: If fewer than two points are provided
    virtual std::vector<Point> Solve(const std::vector<Point>& points) const = 0;

protected:
    ClosestPairFinder();
    void operator=(const ClosestPairFinder&) = delete;
};

}
