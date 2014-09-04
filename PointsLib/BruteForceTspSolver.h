#pragma once

#include "IPointsAlgorithm.h"

namespace PointsLib
{

class BruteForceTspSolver : public IPointsAlgorithm
{
public:
    static BruteForceTspSolver* Create(int numThreads);
    virtual ~BruteForceTspSolver() = 0;

protected:
    BruteForceTspSolver();
    void operator=(const BruteForceTspSolver&) = delete;
};

}