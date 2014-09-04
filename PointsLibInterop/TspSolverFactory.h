#pragma once

namespace PointsLibInterop { interface class ISolver; }

namespace PointsLibInterop
{

public ref class TspSolverFactory abstract sealed
{
public:
    static ISolver^ Create(int numThreads);
};

}