********* Retirement Benefits Calculator *********

This is a simple application that calculates a consumer's life insurance retirement benefits.

On startup, the user will see the Login page where they will be asked to enter his/her credentials.

Note: currently, user for this app is admin only.

********* Application Navigation *********

#Home Menu#
- Home Page of the Retirement Benefits Calculator Application

#Setup Menu#
- Shows a list of Setups
	• The Setup is used for the calculation of retirement benefits
	• Buttons shown in the page:
		(1) Create New
			- Creates a new Setup
		(2) Edit
			- Edits a Setup
		(3) Delete
			- Deletes a Setup
			- Setup will not be deleted if it is assigned to a Consumer

#Consumer Menu#
- Shows a list of Consumers
	• Buttons shown in the page:
		(1) Create New
			- Creates a new Consumer
		(2) Edit
			- Edits Consumer details
			- Can only modify the Consumer's associated Setup and basic salary
		(3) Delete
			- Deletes a Consumer
			- Once a Consumer is deleted, its history of computed results will also be deleted
		(4) History
			- Shows the Consumer's history of retirement benefits computed results (Consumer Benefits History page)
			- Details Button will redirect the user to the Computation Result page
		(5) Setup
			- Shows the Setup associated to the consumer
			- If there is no Setup associated, this button will be hidden

#Compute Benefits Menu#
- Computes a Consumer's retirement benefits
- Shows the Consumer Selection page. This is where the user will choose whose retirement benefits will be calculated
	• Select Button
		- Selects Consumer and redirects the user to the Computation Details page

#History Menu#
- Shows a list of all (irrespective of consumer) benefits computation result
- Buttons shown in the page:
	(1) Details
		- Shows the complete computation result details (Computation Result page)
	(2) Delete Button
		- Deletes the computation result	

#Logout#
- Logs the user out from the application.

#Other Pages/Views#
• Consumer Benefits History (page)
		- Buttons:
		(1) Details
			- Shows the computation result (Computation Result page)
		(2) Delete Button
			- Deletes the computation result
		(3) Back Button
			- Redirects the user back to the list of Consumers page

• Computation Result (page)
	- Shows the benefits computation result with (past, in some cases) values for basic salary and guaranteed issue
	- Back button will redirect the user to the Consumer Benefits History page

• The Computation Details page 
	- Lets the user review the details of the Consumer including its Setup for calculation
		- Compute Button starts the benefits computation
		- On completion of the benefits computation, the user will be redirected to the Computation Result page