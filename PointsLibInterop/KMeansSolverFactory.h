#pragma once

#include "KMeansInitialMeansStrategy.h"

namespace PointsLibInterop { interface class ISolver; }

namespace PointsLibInterop
{

public ref class KMeansSolverFactory abstract sealed
{
public:
    static ISolver^ Create(int k, int repetitions, KMeansInitialMeansStrategy initialMeansStrategy);
};

}