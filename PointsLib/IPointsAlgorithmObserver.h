#pragma once

#include <stdint.h>
#include <vector>

namespace PointsLib { struct Point; }

namespace PointsLib
{

// Interface for an observer of an algorithm that produces a list of points.
// Implementers may throw from any function if they wish to abort the computation.
// Callers of observed algorithms should be prepared to catch these exceptions.
struct IPointsAlgorithmObserver
{
    virtual ~IPointsAlgorithmObserver() = 0;
    
    virtual void OnStarting() = 0;
    
    struct ProgressEventArgs
    {
        ProgressEventArgs();
        ProgressEventArgs(const double* fractionComplete, const std::vector<Point>* provisionalAnswer);
        ProgressEventArgs(const double* fractionComplete_);
        ProgressEventArgs(const std::vector<Point>* provisionalAnswer_);

        // may be null if unknown or inapplicable
        // pointer is NOT guaranteed to remain valid after the function is is passed to returns
        const double* fractionComplete;
        
        // may be null if unknown or inapplicable
        // pointer is NOT guaranteed to remain valid after the function is is passed to returns
        const std::vector<Point>* provisionalAnswer;
    };

    virtual void OnProgress(const ProgressEventArgs& args) = 0;
    
    virtual void OnComplete(const std::vector<Point>& answer) = 0;

    virtual void OnFailure() = 0;
};



// some "extension methods" to make clients' lives easier without cluttering up the interface
void ReportProgress(IPointsAlgorithmObserver& observer, double fractionComplete, const std::vector<Point>& points);
void ReportProgress(IPointsAlgorithmObserver& observer, double fractionComplete);
void ReportProgress(IPointsAlgorithmObserver& observer, const std::vector<Point>& points);

void ReportProgress(IPointsAlgorithmObserver& observer, uint64_t step, uint64_t totalSteps, const std::vector<Point>& points);
void ReportProgress(IPointsAlgorithmObserver& observer, uint64_t step, uint64_t totalSteps);

void ReportProgress(IPointsAlgorithmObserver& observer);

}