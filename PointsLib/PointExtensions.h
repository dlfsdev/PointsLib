#pragma once

#include "Point.h"
#include <vector>

namespace PointsLib
{

// returns the unsigned minimum distance from a point to a line
double DistanceToLine(const Point& point, const Point& l1, const Point& l2);

// find the index of the point in a set that is closest to an input point
size_t FindNearestPoint(const Point& point, const std::vector<Point>& points);

// this can be computed faster than the nonsquared distance and is adequate
// if all you're doing with distances is comparing them
inline double SquaredDistanceBetween(const Point& p1, const Point& p2)
{
    const double dx = p2.x - p1.x;
    const double dy = p2.y - p1.y;
    return dx * dx + dy * dy;
}

inline double DistanceBetween(const Point& p1, const Point& p2)
{
    return std::sqrt(SquaredDistanceBetween(p1, p2));
}

inline bool CompareByX(const Point& p1, const Point& p2)
{
    return p1.x < p2.x;
}

inline bool CompareByY(const Point& p1, const Point& p2)
{
    return p1.y < p2.y;
}

}