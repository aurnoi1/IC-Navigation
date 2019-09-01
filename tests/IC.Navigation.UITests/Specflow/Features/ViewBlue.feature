@xunit:collection(UITests)
Feature: ViewBlue
	I want to validate that controls and features are correctly implemented

Background: 
    Given The application under test has been started
    And The "blue page" has been opened

@view_menu
Scenario: The controls should be displayed in "blue page"
    Then The following controls should be displayed in the current page:
    | usage_name                             |
    | title                                  |
    | button to go back to the previous page |
    | button to open the yellow page         |
