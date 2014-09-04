#pragma once

#include "../PointsLib/IPointsAlgorithmObserver.h"

#include <vcclr.h>

namespace PointsLib { struct Point; }
namespace PointsLibInterop { ref class ProgressReport; }

namespace PointsLibInterop { namespace Private
{

// An implementation of the native IPointsAlgorithmObserver that adapts its notifications into managed IProgress reports
class AlgorithmObserver : public PointsLib::IPointsAlgorithmObserver
{
public:
    // progress: The IProgress that will receive the adapted notifications
    // cancellationToken: If this becomes cancelled, we'll cancel the observed algorithm by throwing a native TaskAbortedException
    //  in response to its next progress notification. The algorithm adapter will need to catch and handle this.
    AlgorithmObserver(System::IProgress<ProgressReport^>^ progress, System::Threading::CancellationToken^ cancellationToken);

    virtual void OnStarting() override;
    virtual void OnProgress(const ProgressEventArgs& args) override;
    virtual void OnComplete(const std::vector<PointsLib::Point>& answer) override;
    virtual void OnFailure() override;

private:
    gcroot<System::IProgress<ProgressReport^>^> m_progress;
    gcroot<System::Threading::CancellationToken^> m_cancellationToken;
};

} }