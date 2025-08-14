import os
from google import genai
from dotenv import load_dotenv

BASE_DIR = os.path.dirname(os.path.abspath(__file__))

load_dotenv()

gemini_api_key = os.getenv("API_KEY")
client = genai.Client(api_key=gemini_api_key)

prompt_path = os.path.join(BASE_DIR, 'ProblemLLMPrompt.txt')
with open(prompt_path, 'r', encoding='utf-8') as file:
    question = file.read()

firstline = input();
lines = question.splitlines()
if lines:
    lines[0] = firstline
question = '\n'.join(lines)

response = client.models.generate_content(
    model="gemini-2.0-flash",
    contents=question
)

lines = response.text.splitlines()

inputStarted = False
outputStarted = False
hintFound = False
statement = ''
hint = ''
inputs = []
outputs = []
current_data = []

for line in lines:
    line = line.strip()
    if not hintFound:
        if line == 'HINT':
            hintFound = True
        else:
            statement += line + '\n'
    elif not inputStarted:
        if line == 'INPUT':
            inputStarted = True
        else:
            hint += line + '\n'
    elif not outputStarted:
        if line == 'OUTPUT':
            outputStarted = True
            inputs.append(" ".join(current_data).strip())
            current_data = []
        elif line == '!':
            inputs.append(" ".join(current_data).strip())
            current_data = []
        else:
            current_data.append(line)
    else:
        if line == '!':
            outputs.append(" ".join(current_data).strip())
            current_data = []
        else:
            current_data.append(line)

if not outputStarted and current_data:
    inputs.append(" ".join(current_data).strip())
elif outputStarted and current_data:
    outputs.append(" ".join(current_data).strip())

problemStatement = statement.strip().replace("\n", "@")
problemInput = " ! ".join(filter(None, inputs))
problemOutput = " ! ".join(filter(None, outputs))
hint = hint.strip()

print(problemStatement)
print(problemInput)
print(problemOutput)
print(hint)
