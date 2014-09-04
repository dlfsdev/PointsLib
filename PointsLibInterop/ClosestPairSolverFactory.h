#pragma once

namespace PointsLibInterop { interface class ISolver; }

namespace PointsLibInterop
{

public ref class ClosestPairSolverFactory abstract sealed
{
public:
    static ISolver^ Create();
};

}