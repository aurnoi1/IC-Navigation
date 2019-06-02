@xunit:collection(UITests)
Feature: ViewMenu
	I want to validate that controls and features are correctly implemented

Background: 
    Given The application under test has been started
    And The "menu view" has been opened

@view_menu
Scenario: The controls should be displayed in "menu view"
    Then The following controls should be displayed in the current view:
    | usage_name                     |
    | title                        |
    | button to open the blue view |
    | button to open the red view  |


@view_menu
Scenario: The "Not Implemented" should be not displayed
   Then The control "not implemented" should not be displayed in the current view
