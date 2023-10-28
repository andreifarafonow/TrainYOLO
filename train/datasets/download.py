from roboflow import Roboflow
rf = Roboflow(api_key="DRngHmEqiN3LgcmLRtvm")
project = rf.workspace("andrey-farafonov-lid0p").project("test7-bcfvg")
dataset = project.version(1).download("yolov8")

print(dataset.location)