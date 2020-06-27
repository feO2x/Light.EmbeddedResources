using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref="Check" /> class provides access to all assertions of Light.GuardClauses.
    /// </summary>
    internal static class Check
    {
        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws an <see cref="ArgumentNullException" />.
        /// </summary>
        /// <param name="parameter">The object reference to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>(this T? parameter, string? parameterName = null, string? message = null)
            where T : class
        {
            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the specified parameter is not null when <typeparamref name="T" /> is a reference type, or otherwise
        /// throws an <see cref="ArgumentNullException" />. PLEASE NOTICE: you should only use this assertion in generic contexts,
        /// use <see cref="MustNotBeNull{T}(T,string,string)" /> by default.
        /// </summary>
        /// <param name="parameter">The value to be checked for null.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T" /> is a reference type and <paramref name="parameter" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNullReference<T>(this T parameter, string? parameterName = null, string? message = null)
        {
#pragma warning disable CS8653 // It's ok if T is resolved to a reference type. This method is usually used in generic contexts.
            if (default(T) != null)
#pragma warning restore CS8653
                return parameter;

            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            return parameter;
        }
    }
}

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// Provides static factory methods that throw default exceptions.
    /// </summary>
    internal static class Throw
    {
        /// <summary>
        /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ArgumentNull(string? parameterName = null, string? message = null) =>
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
    }
}

/* 
License information for JetBrains.Annotations

MIT License
Copyright (c) 2016 JetBrains http://www.jetbrains.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */
namespace JetBrains.Annotations
{
    /// <summary>
    /// Indicates that the value of the marked element could never be <c>null</c>.
    /// </summary>
    /// <example>
    /// <code>
    /// [NotNull] object Foo() {
    ///   return null; // Warning: Possible 'null' assignment
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    internal sealed class NotNullAttribute : Attribute { }

    /// <summary>
    /// Describes dependency between method input and output.
    /// </summary>
    /// <syntax>
    /// <p>Function Definition Table syntax:</p>
    /// <list>
    /// <item>FDT      ::= FDTRow [;FDTRow]*</item>
    /// <item>FDTRow   ::= Input =&gt; Output | Output &lt;= Input</item>
    /// <item>Input    ::= ParameterName: Value [, Input]*</item>
    /// <item>Output   ::= [ParameterName: Value]* {halt|stop|void|nothing|Value}</item>
    /// <item>Value    ::= true | false | null | notnull | canbenull</item>
    /// </list>
    /// If method has single input parameter, it's name could be omitted.<br />
    /// Using <c>halt</c> (or <c>void</c>/<c>nothing</c>, which is the same) for method output
    /// means that the methos doesn't return normally (throws or terminates the process).<br />
    /// Value <c>canbenull</c> is only applicable for output parameters.<br />
    /// You can use multiple <c>[ContractAnnotation]</c> for each FDT row, or use single attribute
    /// with rows separated by semicolon. There is no notion of order rows, all rows are checked
    /// for applicability and applied per each program state tracked by R# analysis.<br />
    /// </syntax>
    /// <examples>
    /// <list>
    /// <item>
    /// <code>
    /// [ContractAnnotation("=&gt; halt")]
    /// public void TerminationMethod()
    /// </code>
    /// </item>
    /// <item>
    /// <code>
    /// [ContractAnnotation("halt &lt;= condition: false")]
    /// public void Assert(bool condition, string text) // regular assertion method
    /// </code>
    /// </item>
    /// <item>
    /// <code>
    /// [ContractAnnotation("s:null =&gt; true")]
    /// public bool IsNullOrEmpty(string s) // string.IsNullOrEmpty()
    /// </code>
    /// </item>
    /// <item>
    /// <code>
    /// // A method that returns null if the parameter is null,
    /// // and not null if the parameter is not null
    /// [ContractAnnotation("null =&gt; null; notnull =&gt; notnull")]
    /// public object Transform(object data) 
    /// </code>
    /// </item>
    /// <item>
    /// <code>
    /// [ContractAnnotation("=&gt; true, result: notnull; =&gt; false, result: null")]
    /// public bool TryParse(string s, out Person result)
    /// </code>
    /// </item>
    /// </list>
    /// </examples>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContractAnnotationAttribute : Attribute
    {
        public ContractAnnotationAttribute([NotNull] string contract) : this(contract, false) { }

        public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
        {
            Contract = contract;
            ForceFullStates = forceFullStates;
        }

        [NotNull]
        public string Contract { get; }

        public bool ForceFullStates { get; }
    }
}