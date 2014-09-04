// Utilities to assist with template metaprogramming

#pragma once

namespace Util
{

// For use in decltype expressions. Like declval, but returns T instead of T&&.
// No definition.
template<typename T> T MakeVal();

// For use in decltype expressions. Like declval, but returns T& instead of T&&.
// No definition.
template<typename T> T& MakeLvalRef();

// Get the type of the elements contained in some iterable collection
template<typename T>
struct element_type
{
    typedef typename std::remove_reference<decltype(*std::begin( MakeLvalRef<T>() ))>::type type;
};

}