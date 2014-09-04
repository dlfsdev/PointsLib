#pragma once

#include "IKMeansInitialMeansStrategy.h"

namespace PointsLib
{

// Assigns each point to a random cluster number and then computes the centroids
IKMeansInitialMeansStrategy* CreateRandomPartitionKMeansInitialMeansStrategy();

}