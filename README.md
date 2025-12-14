This was my internship project as an intern at [Technoperia](https://technoperia.com/).


**Overview**

This project is a program that helps the user practice and develop their data structures and algorithmic skills. It does this by creating custom problems in several ways, all to make sure the problem is as well adopted for the user as possible. This project has allowed me to deeply improve my C# skills and has introduced me to many concepts that were previously foreign to me.

**Problem Generation**

There are four ways the user can generate a problem:<br>
-By giving custom prompts<br>
-By generating a problem based on their unsolved ones<br>
-By choosing to resolve a random previously solved problem<br>
-By choosing to resolve a random previously solved problem with specified categories<br>
-Generating a problem by giving a prompt<br>

For this feature the user is prompted with two inputs: categories and difficulty. An AI service is then activated which will generate a problem with every needed attribute: title, statement, hint, inputs and outputs for test cases. The problem is then stored into the database from where the user can load it and attempt to solve it.

**Generating a problem based on the unsolved**

For this feature the user is not prompted with anything. The program will go through the unsolved questions from where it will select three random categories and one random difficulty. The AI service is then prompted to generate a problem based on the selected parameters.
														
**Revision**

For this feature the user is also not prompted with anything. The program will go through all the solved questions and will randomly select one which the user will attempt to solve again.

**Revision with categories**

This feature is very similar to the previous one. The user is prompted with one input: categories. The program will then go through all the solved problems and it will randomly select one which has the specified category/categories.
						
**Hints**

Each problem will be generated with one hint. It will be hidden by default. The user can choose whether they want to reveal it or not. Each problem will have **only one** hint.

**Problem Review**

This feature allows the user to submit their source code for an AI Review. The code must be for the currently loaded problem. 

**Problem Search**

This feature provides pagination search for all the problems. The user can give several parameters when searching, which are:<br>
-Main search string – Text to look for in the title and statement of the problem<br>
-Categories<br>
-Difficulty<br>
-Problem status<br>
-Time range<br>
-Sort option (sort by status, difficulty and time generated)<br>
Every parameter is optional; however, the user must input at least one of them. 
											
**Problem Testing**

This feature is what allows the user to submit an executable to run through the test cases of a given problem. This way, it will be determined if the user has solved the problem. The problem must be loaded beforehand. If one test-case fails, the execution will stop. The remaining test-cases will not be tested. In the end the problem will be marked as either solved or unsolved. A test-case can fail in three different ways:<br>
-Time Limit Exceeded<br>
-Memory Limit Exceeded<br>
-Incorrect Answer<br>
