#include "stdafx.h"
#include "IPointsAlgorithmObserver.h"

namespace PointsLib
{

IPointsAlgorithmObserver::~IPointsAlgorithmObserver()
{
}



IPointsAlgorithmObserver::ProgressEventArgs::ProgressEventArgs()
    : fractionComplete(nullptr)
    , provisionalAnswer(nullptr)
{
}

IPointsAlgorithmObserver::ProgressEventArgs::ProgressEventArgs(const double* fractionComplete_, const std::vector<Point>* provisionalAnswer_)
    : fractionComplete(fractionComplete_)
    , provisionalAnswer(provisionalAnswer_)
{
}

IPointsAlgorithmObserver::ProgressEventArgs::ProgressEventArgs(const double* fractionComplete_)
    : fractionComplete(fractionComplete_)
    , provisionalAnswer(nullptr)
{
}

IPointsAlgorithmObserver::ProgressEventArgs::ProgressEventArgs(const std::vector<Point>* provisionalAnswer_)
    : fractionComplete(nullptr)
    , provisionalAnswer(provisionalAnswer_)
{
}



void ReportProgress(IPointsAlgorithmObserver& observer, double fractionComplete, const std::vector<Point>& points)
{
    observer.OnProgress(IPointsAlgorithmObserver::ProgressEventArgs(&fractionComplete, &points));
}

void ReportProgress(IPointsAlgorithmObserver& observer, double fractionComplete)
{
    observer.OnProgress(IPointsAlgorithmObserver::ProgressEventArgs(&fractionComplete));
}

void ReportProgress(IPointsAlgorithmObserver& observer, const std::vector<Point>& points)
{
    observer.OnProgress(IPointsAlgorithmObserver::ProgressEventArgs(&points));
}

void ReportProgress(IPointsAlgorithmObserver& observer, uint64_t step, uint64_t totalSteps, const std::vector<Point>& points)
{
    ReportProgress(observer, (double)step / totalSteps, points);
}

void ReportProgress(IPointsAlgorithmObserver& observer, uint64_t step, uint64_t totalSteps)
{
    ReportProgress(observer, (double)step / totalSteps);
}

void ReportProgress(IPointsAlgorithmObserver& observer)
{
    observer.OnProgress(IPointsAlgorithmObserver::ProgressEventArgs());
}

}