#pragma once

#include "Point2d.h"
#include "ProgressReport.h"

namespace PointsLib { struct IPointsAlgorithm; }

namespace PointsLibInterop
{

public interface class ISolver
{
    System::Collections::Generic::IEnumerable<Point2d^>^ Solve(
        System::Collections::Generic::IEnumerable<Point2d^>^ points,
        System::IProgress<ProgressReport^>^ progress,
        System::Threading::CancellationToken^ cancellationToken);

    property bool ReportsDefiniteProgress { bool get(); }
};

}