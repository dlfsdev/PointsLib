#pragma once

#include "TemplateUtil.h"

#include <iterator>
#include <vector>

namespace Util
{

// Make a copy of any copy-constructable object
template<typename T>
inline T MakeCopy(const T& o)
{
    return T(o);
}

// Convert an arbitrary iterable collection into a vector
template<typename T>
auto ToVector(T& collection) -> std::vector<typename element_type<T>::type>
{
    return std::vector<typename element_type<T>::type>(std::begin(collection), std::end(collection));
}

}