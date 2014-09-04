#pragma once

#include <utility>
#include <vector>

namespace PointsLib
{

struct Point
{
    double x;
    double y;

    Point();
    Point(double x, double y);

    bool operator==(const Point& other) const;
    bool operator!=(const Point& other) const;

    bool operator<(const Point& other) const;
};

}