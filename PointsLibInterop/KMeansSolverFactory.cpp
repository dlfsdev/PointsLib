#include "stdafx.h"
#include "KMeansSolverFactory.h"

#include "NativeAlgorithmSolver.h"

#include "../PointsLib/ForgyKMeansInitialMeansStrategy.h"
#include "../PointsLib/PlusPlusKMeansInitialMeansStrategy.h"
#include "../PointsLib/RandomPartitionKMeansInitialMeansStrategy.h"
#include "../PointsLib/KMeansSolver.h"

using namespace PointsLib;
using namespace PointsLibInterop::Private;

namespace PointsLibInterop
{

std::shared_ptr<IKMeansInitialMeansStrategy> CreateInitialMeansStrategy(KMeansInitialMeansStrategy strategy)
{
    switch(strategy)
    {
    case KMeansInitialMeansStrategy::Forgy:
        return std::shared_ptr<IKMeansInitialMeansStrategy>(CreateForgyKMeansInitialMeansStrategy());
    case KMeansInitialMeansStrategy::PlusPlus:
        return std::shared_ptr<IKMeansInitialMeansStrategy>(CreatePlusPlusKMeansInitialMeansStrategy());
    case KMeansInitialMeansStrategy::RandomPartition:
        return std::shared_ptr<IKMeansInitialMeansStrategy>(CreateRandomPartitionKMeansInitialMeansStrategy());
    default:
        throw std::exception("Unknown initial means strategy");
    }
}

ISolver^ KMeansSolverFactory::Create(int k, int repetitions, KMeansInitialMeansStrategy initialMeansStrategy)
{
    return gcnew NativeAlgorithmSolver(PointsLib::KMeansSolver::Create(k, repetitions,
        CreateInitialMeansStrategy(initialMeansStrategy)));
}

}