#pragma once

#include "ObserverSet.h"
#include "IPointsAlgorithmObserver.h"

namespace PointsLib
{

// Convenience class that wraps up a set of IPointsAlgorithmObservers
class PointsAlgorithmObservers : public IPointsAlgorithmObserver, public Util::ObserverSet<IPointsAlgorithmObserver>
{
public:
    PointsAlgorithmObservers();
    ~PointsAlgorithmObservers();

    // Call the given IPointsAlgorithmObserver function with the given argument(s) on all registered observers
    virtual void OnStarting() override;
    virtual void OnProgress(const IPointsAlgorithmObserver::ProgressEventArgs& args) override;
    virtual void OnComplete(const std::vector<Point>& answer) override;
    virtual void OnFailure() override;
};

}