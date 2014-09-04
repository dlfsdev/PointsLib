#include "stdafx.h"
#include "TspSolverFactory.h"

#include "NativeAlgorithmSolver.h"

#include "../PointsLib/BruteForceTspSolver.h"

using namespace PointsLibInterop::Private;

namespace PointsLibInterop
{

ISolver^ TspSolverFactory::Create(int numThreads)
{
    return gcnew NativeAlgorithmSolver(PointsLib::BruteForceTspSolver::Create(numThreads));
}

}