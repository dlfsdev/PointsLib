#pragma once
#include "ISolver.h"

namespace PointsLib { struct IPointsAlgorithm; }
namespace PointsLibInterop { ref class Point2d; }
namespace PointsLibInterop { ref class ProgressReport; }

namespace PointsLibInterop { namespace Private
{

private ref class NativeAlgorithmSolver sealed : ISolver
{
public:
    NativeAlgorithmSolver(PointsLib::IPointsAlgorithm* algorithm);
    ~NativeAlgorithmSolver();

    virtual System::Collections::Generic::IEnumerable<Point2d^>^ Solve(
        System::Collections::Generic::IEnumerable<Point2d^>^ points,
        System::IProgress<ProgressReport^>^ progress,
        System::Threading::CancellationToken^ cancellationToken);

    virtual property bool ReportsDefiniteProgress { bool get(); }
        
private:
    PointsLib::IPointsAlgorithm* m_algorithm;
};

} }