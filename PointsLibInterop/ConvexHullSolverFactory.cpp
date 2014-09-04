#include "stdafx.h"
#include "ConvexHullSolverFactory.h"

#include "NativeAlgorithmSolver.h"

#include "../PointsLib/QuickHullSolver.h"

using namespace PointsLibInterop::Private;

namespace PointsLibInterop
{

ISolver^ ConvexHullSolverFactory::Create()
{
    return gcnew NativeAlgorithmSolver(PointsLib::QuickHullSolver::Create());
}

}