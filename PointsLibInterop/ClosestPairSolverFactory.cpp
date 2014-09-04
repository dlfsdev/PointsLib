#include "stdafx.h"
#include "ClosestPairSolverFactory.h"

#include "NativeAlgorithmSolver.h"

#include "../PointsLib/ClosestPairFinder.h"

using namespace PointsLibInterop::Private;

namespace PointsLibInterop
{

ISolver^ ClosestPairSolverFactory::Create()
{
    return gcnew NativeAlgorithmSolver(PointsLib::ClosestPairFinder::Create());
}

}