#include "stdafx.h"
#include "PointExtensions.h"

#include <limits>

namespace PointsLib
{

double DistanceToLine(const Point& point, const Point& l1, const Point& l2)
{
    const double x0 = point.x;
    const double y0 = point.y;

    const double x1 = l1.x;
    const double y1 = l1.y;

    const double x2 = l2.x;
    const double y2 = l2.y;

    const double dx = x2 - x1;
    const double dy = y2 - y1;

    const double numerator = std::abs(dy*x0 - dx*y0 - x1*y2 + x2*y1);
    const double denominator = std::sqrt(dx*dx + dy*dy);

    const double answer = numerator / denominator;
    return answer;
}

size_t FindNearestPoint(const Point& point, const std::vector<Point>& points)
{
    if(points.empty())
        throw std::exception("Must provide at least 1 point");

    size_t closestIndex = 0;
    double closestDistance = std::numeric_limits<double>::max();
            
    for(size_t i = 0; i < points.size(); ++i)
    {
        const double thisDistance = SquaredDistanceBetween(point, points[i]);
        if(thisDistance < closestDistance)
        {
            closestIndex = i;
            closestDistance = thisDistance;
        }
    }

    return closestIndex;
}

}