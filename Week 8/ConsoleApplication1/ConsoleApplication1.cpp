// ConsoleApplication1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <string>
#include <cmath>

int main()
{
    std::cout << "Hello World!\n";
	double x = _CMATH_::sqrt(16.0); // Using the cmath library to calculate the square root of 16
	std::cout << "The square root of 16 is: " << x << std::endl;
	// Example of using a string
	std::string greeting = "Welcome to the C++ program!";
	std::cout << greeting << std::endl;
	// Example of using a mathematical operation
	double result = 5.0 * 3.0;
	std::cout << "The result of 5.0 * 3.0 is: " << result << std::endl;
	double theMax = std::max(10.0, 20.0); // Using the max function from the cmath library
	std::cout << "The maximum of 10.0 and 20.0 is: " << theMax << std::endl;	
	return 0;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
