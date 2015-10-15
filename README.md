Improved by [Mathew Sachin](http://github.com/MathewSachin).

The Included MathParser is an Object-Oriented Version of
[MathParser_TK by Yerzhan Kalzhani at CodeProject](http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp).  
It uses [Reverse-Polish Notation(RPN)](https://en.wikipedia.org/wiki/Reverse_Polish_notation)
to parse Mathematical Expressions. 

# Differences from the Original Source
* ObjectOrientation using Tokens
* Easily Define Variables, Operators, Functions even at Runtime
* Value of Variables can be changed dynamically, even after a function is parsed and RPN is generated
* `RPN Caching` for Reusable Expressions
* `Comma Seperated Parameter Lists` for MultiArgument Functions
* Support for `TernaryFunctions`
* Support for `PostfixFunctions` like Factorial
* AngleType Includes `Grades`
* Predefined `Combinatorial, Inverse and RoundOff` Functions
* Bypasses `LexicalAnalysisRPN()`, storing the RPN as as `List<Token>` instead of `System.String` for faster Evaluation

# Using the Parser

Tokens
-------------------------------------------------------------
A Token is the Smallest discrete unit for the Parser.  
It may be an Operator, a Function, a Constant, or a Variable

### Hierarchy
* `Token`
	* `Operator`
		* `UnaryOperator`
		* `BinaryOperator`
	* `Function`
		* `UnaryFunction`
			* `TrigonometricFunction`
			* `PostfixFunction`
		* `BinaryFunction`
		* `TernaryFunction`
	* `Constant`
	* `Variable`

### Priority
The Value of Priority determines the order in which Tokens are Parsed.  
Tokens with Higher Priority are parsed first.  
You can use `(` and `)` to Override the default priorities.  
The Priority of Tokens can be compared using `>=` and `<=` Operators

### Keyword
The Keyword is the text representation of the Token which would be entered by the user.

### Operator
Predefined Operators: 

Keyword | What it does | Kind | Priority
--------|--------------|------|-----------
+ | Addition | Binary, Unary | 6(Unary), 2(Binary)
- | Subtraction | Binary, Unary | 6(Unary), 2(Binary)
* | Multiplication | Binary | 4
/ | Division | Binary | 4
^ | Power | Binary | 8
% | Remainder | Binary | 8

#### UnaryOperator
UnaryOperator takes only one Parameter.  
```csharp
Constructor: UnaryOperator(string Keyword, Func<double, double> Procedure);
```

e.g. Defining a Unary Operator (~ Negation)
```csharp
MathParser P = new MathParser(); // Create a Parser
UnaryOperator N = new UnaryOperator("~", (x) => -x); // Create the Operator
P.Operators.Add(N); // Add it to the Parser's Operator List
```

#### BinaryOperator
BinaryOperator takes Two Parameters
```csharp
Constructor: BinaryOperator(string Keyword, Func<double, double, double> Procedure);
```

e.g. Defining a Binary Operator (+ Addition)
```csharp
MathParser P = new MathParser(); // Create a Parser
BinaryOperator A = new BinaryOperator("+", (x, y) => x + y); // Create the Operator
P.Operators.Add(A); // Add it to the Parser's Operator List
```

### Function
The Arguments of a Function must be in `Paranthesis`

Predefined Functions:

Keyword | What it does | Kind
--------|--------------|-----
sqrt | Square Root | Unary
sin | Sine | Trigonometric
cos | Cosine | Trigonometric
tan | Tangent | Trigonometric
sec | Secant | Trigonometric
cosec | Cosecant | Trigonometric
cot | Cotangent | Trigonometric
asin | Arc Sine | Unary
acos | Arc Cosine | Unary
atan | Arc Tangent | Unary
sinh | Hyperbolic Sine | Unary
cosh | Hyperbolic Cosine | Unary
tanh | Hyperbolic Tangent | Unary
ln | Natural Logarithm | Unary
exp | Exponential | Unary
abs | Absolute Value | Unary
floor | Greatest Integer | Unary
ceiling | Ceiling | Unary
round | Round | Unary
sign | Sign | Unary
truncate | Truncate | Unary
factorial | Factorial | Unary
log | Logarithm | Binary
max | Maximum | Binary
min | Minimum | Binary
c | Combinations | Binary
p | Permutations | Binary
clip | Clips a Value between a Maximum and a Minimum | Ternary

#### UnaryFunction
UnaryFuncion takes only one Parameter
```csharp
Constructor: UnaryFunction(string Keyword, Func<double, double> Procedure);
```

#### PostfixFunction
PostfixFunction is a Unary Function in which the Operand comes before the Function.  
`Paranthesis is not necessary for this kind of Function.`  
The most common example is the `Factorial (!)`.  
```csharp
// Using Factorial
new MathParser().Evaluate("2!");
new MathParser().Evaluate("(2)!");
new MathParser().Evaluate("(2+6)!");
```

#### BinaryFunction
BinaryFunction takes Two Parameters
```csharp
Constructor: BinaryFunction(string Keyword, Func<double, double, double> Procedure);
```

e.g. Invoking a Binary Function - log
```csharp
// Finding log of 2 to the base 10 = 0.3010...
new MathParser().Evaluate("log(2, 10)"); // First Way
new MathParser().Evaluate("(2)log(10)"); // Second Way
new MathParser().Evaluate("log((2) 10)"); // Third Way
new MathParser().Evaluate("log(2 (10))"); // Fourth Way
```

#### TrigonometricFunction
TrigonometricFunction takes the Angle and its type as Parameters.  
The Processing is done by a `Func<double, double>` accepting the input in Radians.  
The Conversion from the specified AngleType to Radians is automatically handled.  
```csharp
Constructor: TrigonometricFunction(string Keyword, Func<double, double> Procedure)
```

AngleType Parameter of the Parser can have values of the `enum AngleType { Radians, Degrees, Grades }`

#### TernaryFunction
TernaryFunction takes three Parameters  
```csharp
Constructor: TernaryFunction(string Keyword, Func<double, double, double, double> Procedure);
```

### Constants
Keyword | Value
--------|---------
e | Math.E
pi | Math.Pi

### Variable
A Variable is an item having a Value that can be changed dynamically.  
The Value of the Variable can also be changed even after the Parsing is Complete and RPN is generated.  

# RPN Caching
The RPN Expression is Cached after you Invoke the `Parse(string)` or `Evaluate(string)` method.  
You could modify the Variables if needed and then ReEvaluate it faster using the Parameterless `Evaluate()` method.

e.g. Using the Parser for Summation of a Function (sin(x) from 0 to 10)
```csharp
MathParser P = new MathParser();

Variable V = new Variable("x", 0);

P.Variables.Add(V);

P.Parse("sin(x)"); // Parse the Expression only once, cache the RPN

int Value = 0; // Final Value of the Sum

for (int i = 0; i <= 10; ++i)
{
    V.Value = i; // Set the value of the variable
    Value += P.Evaluate(); // Evaluate the RPN
}
```

e.g. The Ulimate Example - Using the Parser for Integration of an Expression from a to b.  
(Code from Sigmation.cs in Equamatics)
```csharp
public static double LeftHandIntegration(string Expression, double a, double b, char IterationVariable)
{
    var parser = new MathParser();

    var Var = new Variable(IterationVariable.ToString(), 0);

    parser.Variables.Add(Var);

    parser.Parse(Expression);

    Func<double, double> f = (x) =>
    {
        Var.Value = x;
        return parser.Evaluate();
    };

    int Strips = 100;

    for (int i = (int)a; i < b; i++)
        Strips = (Strips > f(i)) ? Strips : (int)f(i);

    double h = (b - a) / Strips,
        Result = 0;

    for (int i = 0; i < Strips; i++) Result += h * f(a + i * h);

    return Result;
}
```

# How it Works? - The Algorithm
The Parses Converts the mathematical expression provided in `Infix Notation` to `Reverse-Polish Notation (RPN)`.  
It then evaluates the RPN.

1. `FormatString()` removes WhiteSpaces and Checks for Unbalanced Paranthesis and throws an Exception if any found.
2. `ConvertToRPN()` converts infix to RPN and caches it in `List<Token>` RPNExpression;
	* `LexicalAnalysisInfixNotation()` parses Tokens from Infix Expression.
	* `RPNGenerator` generates the RPN TokenList and Stack
		* If Token is a `Number` or a `Variable`, it is pushed to List
		* Else If it is a `Function`, it is pushed to Stack
		* Else If it is `(`, it is pushed to Stack
		* Else If it is `)`, pop elements from stack to list until we find `(`,
		  and add to list a function if any in the peek of stack
		* Else Move Elements from stack to list while their priority is greater than Token
		* Pop Operators which must be the only kind of items present in Stack to List
3. `RPNEvaluator` evaluates the RPN expression and returns the result as a `System.Double`
	* If Token is a `Number`, Push it to the Stack
	* Else If Token is a `Variable`, Push its value to the Stack
	* Else If Token is a `UnaryOperator`, Pop a `double` from Stack, Invoke the Operator and Push its Result back to Stack
	* Else If Token is a `TrigonometricFunction`, proceed as for a `UnaryOperator` while passing `AngleType` also on `Invoke()`
	* Else If Token is a `UnaryFunction` or a `PostfixFunction`, proceed as for `UnaryOperator`
	* Else If Token is a `BinaryFunction` or a `BinaryOperator`, Pop two values from the Stack, Invoke the Token, Push the Result back to Stack
	* Else If Token is a `TernaryFunction`, Pop three Values and proceed as above
	* End: The Stack Should Contain only one Operand - `The Result`