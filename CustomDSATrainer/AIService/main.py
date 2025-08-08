from google import genai
from dotenv import load_dotenv
import os

load_dotenv()

gemini_api_key = os.getenv("API_KEY")
client = genai.Client(api_key = gemini_api_key)
question = ""

with open('LLMPrompt.txt', 'r') as file:
    question = file.read()

response = client.models.generate_content(
    model = "gemini-2.0-flash", contents = question
)

#print(response.text)
lines = response.text.splitlines()

inputStarted = False
outputStarted = False

statement = ""
input = ""
output = ""

currentInput = 1

for line in lines:
    if inputStarted == False:
        if line == 'INPUT':
            inputStarted = True
            continue
        else:
            statement += line
            statement += '\n'
    elif outputStarted == False:
        if line == 'OUTPUT':
            outputStarted = True
            continue
        else:
            if line == '!':
                nameOfFile = "Task/Inputs/" + str(currentInput)
                with open(nameOfFile, 'w') as f:
                    f.write(input)
                    input = ""
                currentInput += 1
            else:
                input += line
                input += '\n'
    else:
        output += line
        input += '\n'
        
with open('Task/Statement.txt', 'w') as statementFile:
    statementFile.write(statement)

with open('Task/InputNumbers.txt', 'w') as inputFile:
    inputFile.write(input)
    
with open('Task/ExpectedOutput.txt', 'w') as outputFile:
    outputFile.write(output)