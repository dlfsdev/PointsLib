#pragma once

#include "IPointsAlgorithm.h"
#include <memory>

namespace PointsLib { struct IKMeansInitialMeansStrategy; }

namespace PointsLib
{

class KMeansSolver : public IPointsAlgorithm
{
public:
    static KMeansSolver* Create(int k, int repetitions,
        const std::shared_ptr<const IKMeansInitialMeansStrategy>& initialMeansStrategy);
    
    virtual ~KMeansSolver() = 0;

protected:
    KMeansSolver();
    void operator=(const KMeansSolver&) = delete;
};

}