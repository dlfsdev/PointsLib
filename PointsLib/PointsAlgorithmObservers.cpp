#include "stdafx.h"
#include "PointsAlgorithmObservers.h"

namespace PointsLib
{

PointsAlgorithmObservers::PointsAlgorithmObservers()
{
}


PointsAlgorithmObservers::~PointsAlgorithmObservers()
{
}

void PointsAlgorithmObservers::OnStarting()
{
    ForEachObserver(
        [](IPointsAlgorithmObserver& observer)
        {
            observer.OnStarting();
        });
}

void PointsAlgorithmObservers::OnProgress(const IPointsAlgorithmObserver::ProgressEventArgs& args)
{
    ForEachObserver(
        [&](IPointsAlgorithmObserver& observer)
        {
            observer.OnProgress(args);
        });
}

void PointsAlgorithmObservers::OnComplete(const std::vector<Point>& answer)
{
    ForEachObserver(
        [&](IPointsAlgorithmObserver& observer)
        {
            observer.OnComplete(answer);
        });
}

void PointsAlgorithmObservers::OnFailure()
{
    ForEachObserver(
        [&](IPointsAlgorithmObserver& observer)
        {
            observer.OnFailure();
        });
}

}