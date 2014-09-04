#pragma once

#include "IKMeansInitialMeansStrategy.h"

namespace PointsLib
{

// Randomly select k points to be the initial clusters where each point's likelihood of being selected
// is proportional to its (squared) distance to the nearest already-selected cluster center
IKMeansInitialMeansStrategy* CreatePlusPlusKMeansInitialMeansStrategy();

}