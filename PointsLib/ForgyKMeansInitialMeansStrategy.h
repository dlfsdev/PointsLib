#pragma once

#include "IKMeansInitialMeansStrategy.h"

namespace PointsLib
{

// Randomly select k points to be the initial cluster centers with uniform probability
IKMeansInitialMeansStrategy* CreateForgyKMeansInitialMeansStrategy();

}