@xunit:collection(UITests)
Feature: ViewMenu
	I want to validate that controls and features are correctly implemented

Background: 
    Given The application under test has been started
    And The "menu page" has been opened

@view_menu
Scenario: The controls should be displayed in "menu page"
    Then The following controls should be displayed in the current page:
    | usage_name                     |
    | title                        |
    | button to open the blue page |
    | button to open the red page  |


@view_menu
Scenario: The "Not Implemented" should be not displayed
   Then The control "not implemented" should not be displayed in the current page
