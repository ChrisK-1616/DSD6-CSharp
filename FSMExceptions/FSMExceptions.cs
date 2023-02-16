/**
Author: Chris Knowles
Date: Jan 2023
Copyright: Copperhead Labs, (c)2023
File: FSMExceptions.cs
Version:    1.0.0
Notes: 
*/

namespace FSMExceptions
{
    public class GuardFailedException : Exception
    {

    }

    public class StateNotFoundError : Exception
    {

    }
    public class NoTransitionsError : Exception
    {

    }
    public class TransitionNotFoundError : Exception
    {

    }
    public class DuplicateTransitionError : Exception
    {

    }
}