#include "stdafx.h"
#include "AlgorithmObserver.h"

#include "PointExtensions.h"
#include "ProgressReport.h"
#include "TaskAbortedException.h"

#include "../PointsLib/Point.h"

using namespace PointsLib;

namespace PointsLibInterop { namespace Private
{

AlgorithmObserver::AlgorithmObserver(System::IProgress<ProgressReport^>^ progress, System::Threading::CancellationToken^ cancellationToken)
    : m_progress(progress)
    , m_cancellationToken(cancellationToken)
{
}

void AlgorithmObserver::OnStarting()
{
}

void AlgorithmObserver::OnProgress(const ProgressEventArgs& args)
{
    if(m_cancellationToken->IsCancellationRequested)
        throw TaskAbortedException();

    ProgressReport^ report = gcnew ProgressReport();
        
    report->Progress = args.fractionComplete == nullptr ? System::Nullable<double>()
        : System::Nullable<double>(*args.fractionComplete);
        
    report->ProvisionalAnswer = args.provisionalAnswer == nullptr ? nullptr
        : MakeManagedPoints(*args.provisionalAnswer);

    m_progress->Report(report);
}

void AlgorithmObserver::OnComplete(const std::vector<Point>& answer)
{
    ProgressEventArgs args;
    double fractionComplete = 1.0;
    args.fractionComplete = &fractionComplete;
    args.provisionalAnswer = &answer;
    OnProgress(args);
}

void AlgorithmObserver::OnFailure()
{
}

} }