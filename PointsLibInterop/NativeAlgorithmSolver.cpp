#include "stdafx.h"
#include "NativeAlgorithmSolver.h"

#include "AlgorithmObserver.h"
#include "Point2d.h"
#include "PointExtensions.h"
#include "ProgressReport.h"
#include "TaskAbortedException.h"

#include "../PointsLib/IPointsAlgorithm.h"

#include <assert.h>
#include <exception>

using namespace PointsLib;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;

namespace PointsLibInterop { namespace Private
{

NativeAlgorithmSolver::NativeAlgorithmSolver(IPointsAlgorithm* algorithm)
    : m_algorithm(algorithm)
{
}

NativeAlgorithmSolver::~NativeAlgorithmSolver()
{
    delete m_algorithm;
}

IEnumerable<Point2d^>^ NativeAlgorithmSolver::Solve(IEnumerable<Point2d^>^ points, IProgress<ProgressReport^>^ progress,
    CancellationToken^ cancellationToken)
{
    try
    {
        std::shared_ptr<AlgorithmObserver> observer(new AlgorithmObserver(progress, cancellationToken));
        m_algorithm->AddObserver(observer);
        return MakeManagedPoints(m_algorithm->Solve(MakeNativePoints(points)));
    }
    catch(TaskAbortedException&)
    {
        assert(cancellationToken->IsCancellationRequested);
        throw gcnew System::OperationCanceledException(*cancellationToken);
    }
    catch(std::exception& e)
    {
        throw gcnew System::Exception(gcnew System::String(e.what()));
    }
}

bool NativeAlgorithmSolver::ReportsDefiniteProgress::get()
{
    return m_algorithm->ReportsDefiniteProgress();
}
        
} }