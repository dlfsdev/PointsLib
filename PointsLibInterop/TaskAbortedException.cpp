#include "stdafx.h"
#include "TaskAbortedException.h"

namespace PointsLibInterop { namespace Private
{

TaskAbortedException::TaskAbortedException()
    : std::exception()
{
}

TaskAbortedException::TaskAbortedException(const TaskAbortedException& other)
    : std::exception(other)
{
}

TaskAbortedException::~TaskAbortedException()
{
}

} }