#pragma once

namespace PointsLibInterop
{

public ref class Point2d sealed
{
public:
    Point2d(double x, double y)
    {
        X = x;
        Y = y;
    }

    property double X;
    property double Y;
};

}