#1 : 

As asked I implemented an UI Optimization. By looking the next steps and the project, 
I took the decision to go to an easy and fast optimization : using Sprite Atlas to wrap
all UI Sprites in an Atlas, creating only one texture for all UI Sprites. I've already seen
big performance improvments on some projects.

For the pooling system I've implemented a small pool manager similar using two dictionnaries 
to save a list of each kind of pooled elements. One dictionnary stocks enables ones, will the
other stocks disabled ones. It can be use for any objects. 


#2 : 

I've implemented a sytem using Scriptable Objects to store "Missions" and "Reward". A Mission 
Manager stocks all Missions in a list and can chose randomly one easy, medium and hard one. In
order to be used anywhere I've decided to use a string Id, that the targeted element as to put in a event when needed. The manager listen the event and parse current missions list to find the targeted mission and updated value.

To create a new mission, a new Scriptable Object "Mission" is created, an Id has to be written and
be send at the correct moment in code. It can be used in differents projects as required, without major
modifications and creating new missions or reward isn't difficult.