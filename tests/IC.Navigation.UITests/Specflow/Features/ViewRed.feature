@xunit:collection(UITests)
Feature: ViewRed
	I want to validate that controls and features are correctly implemented

Background: 
    Given The application under test has been started
    And The "red page" has been opened

@view_menu
Scenario: The controls should be displayed in "red page"
    Then The following controls should be displayed in the current page:
    | usage_name                             |
    | title                                  |
    | button to go back to the previous page |
    | button to open the yellow page         |
