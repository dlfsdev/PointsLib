#pragma once

namespace PointsLibInterop { interface class ISolver; }

namespace PointsLibInterop
{

public ref class ConvexHullSolverFactory abstract sealed
{
public:
    static ISolver^ Create();
};

}