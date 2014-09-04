#pragma once

#include <exception>

namespace PointsLibInterop { namespace Private
{

class TaskAbortedException : public std::exception
{
public:
    TaskAbortedException();
    TaskAbortedException(const TaskAbortedException& other);

    ~TaskAbortedException();
};

} }