@xunit:collection(UITests)
Feature: Navigation
	As a tester
	I want to the system to be able to navigate through the UI application 

Background: 
    Given The application under test has been started

@mytag
Scenario: Reuse steps declares in different Step classes
    # The dipose of each Step classes are called at the end of the scenario.
    # This example is explicitly forcing a path and should not be used as Gherkin example
    And The "menu page" has been opened
    Then The control "button to open the red page" should be displayed in the current page
    Given The "blue page" has been opened
    When I navigate to "menu page"
    Then The "menu page" should be opened
    And The control "button to open the red page" should be displayed in the current page
    And The control "fake name" should not be displayed in the current page

@mytag
Scenario: Navigate from the "menu page" to "blue page"
	Given The "menu page" has been opened
	When The "button to open the blue page" is pressed in current page
	Then The "blue page" should be opened

@mytag
Scenario: Navigate back to the "menu page" from "blue page"
	Given The "blue page" has been opened
	When The "button to go back to the previous page" is pressed in current page
	Then The "menu page" should be opened

@mytag
Scenario: Navigate to the "yellow page" from "blue page"
	Given The "blue page" has been opened
	When The "button to open the yellow page" is pressed in current page
	Then The "yellow page" should be opened

@mytag
Scenario Outline: The back button in "yellow page" should open the previous page
    Given The <view_before_yellow> has been opened
    When The <button_to_open_yellow_view> is pressed in current page
	Then The "yellow page" should be opened
    When The "button to go back to the previous page" is pressed in current page
    Then The <view_before_yellow> should be opened

   Examples: 
   | view_before_yellow | button_to_open_yellow_view           |
   | "menu page"        | "button to open the yellow page"     |
   | "blue page"        | "button to open the yellow page"     |
   | "red page"         | "button to open the yellow page"     |

@mytag
Scenario Outline: Navigate from a page to another and come back to the original page.
    Given The <original_view> has been opened
    When I navigate to <destination_view>
    Then The <destination_view> should be opened
    When I navigate to <original_view>
    Then The <original_view> should be opened

   Examples:
   | original_view | destination_view |
   | "menu page"   | "yellow page"    |
   | "blue page"   | "red page"       |
   | "red page"    | "yellow page"    |
   | "yellow page" | "menu page"      |


@mytag
Scenario Outline: Resolve navigation when an action can open more than one page.
    When I navigate to <view_opening_yellow_view>
    And I navigate to "yellow page"
    And I navigate to <a_different_than_view_opening_yellow_view>
    Then The <a_different_than_view_opening_yellow_view> should be opened

    Examples: 
    | view_opening_yellow_view | a_different_than_view_opening_yellow_view |
    | "blue page"              | "red page"                                |
    | "red page"               | "blue page"                               |
    | "menu page"              | "blue page"                               |
    | "menu page"              | "red page"                                |
    | "red page"               | "menu page"                               |
    | "blue page"              | "menu page"                               |
