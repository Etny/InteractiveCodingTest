## **Interactive Coding Test**
A program that tasks the user with solving a set of coding problems designed to test their level of expertise with C#.

## **Usage**

### Running the program
Run the `script/run.*` script appropriate for your OS (.bat for windows, .sh for unix). This will cd you into `workspace/` and start running the test. If you want to stop the test, or the test crashes for part way through (it's not finished!), you can use this same method to restart the test program, which will let you continue from where you left off.

### Arguments
By default the program will simply start the test. The flag `-p` will prompt an administator to enter a validation password at the beginning of the test, which will then be used to sign the tests results. The `-v` flag will not start the test, but instead simply prompt an admin for a password which will then be used to validate the result's signature.