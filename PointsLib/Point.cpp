#include "stdafx.h"
#include "Point.h"

#include <cmath>

namespace PointsLib
{

Point::Point()
    : x(0.0)
    , y(0.0)
{
}

Point::Point(double x_, double y_)
    : x(x_)
    , y(y_)
{
}

bool Point::operator==(const Point& other) const
{
    return other.x == x && other.y == y;
}

bool Point::operator!=(const Point& other) const
{
    return !(*this == other);
}

bool Point::operator<(const Point& other) const
{
    if(x != other.x)
        return x < other.x;
    return y < other.y;
}

}